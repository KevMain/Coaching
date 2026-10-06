# CONSTRAINTS: Paceframe Coaching website

Goal: practical, measurable quality constraints for a small UK running-coaching website (mobile-first, accessible, fast, GDPR-conscious enquiry handling, secure form processing, SEO basics, and straightforward deployment).

ASSUMPTIONS
- Small server-rendered ASP.NET Core site, no public user accounts in v1.
- Hosting: Azure App Service.
- Contact form sends emails via SendGrid; no database storage in v1.

1) Mobile‑first
- Target: pages render and are usable at 320px width (small phones) and scale up.
- Verification: manual responsive check + Chrome DevTools device emulation per page.
- Enforcement: page layout/critical CTA must be accessible and visible within the 320–480px viewport without horizontal scroll.

2) Accessibility (WCAG)
- Target: WCAG 2.1 AA for all public pages.
- Concrete checks:
  - Automated: axe or pa11y run in CI; no new violations introduced.
  - Lighthouse accessibility score >= 90 for Home and Contact pages.
  - Color contrast >= 4.5:1 for body text; large text >= 3:1.
  - All images include alt text; interactive controls keyboard-focusable; form fields labelled.
- Enforcement: CI fails if automated accessibility checks find any critical/serious violations. Manual review required for 'moderate' issues before merge.

3) Performance
- Targets (mobile emulation, throttled network):
  - Lighthouse Performance score >= 80 on Home and Contact pages.
  - Largest Contentful Paint (LCP) <= 2.5s.
  - Total page weight (compressed) <= 500 KB for primary pages.
- Guidelines:
  - Serve optimized images (WebP or responsive srcset), defer non-essential JS, inline critical CSS where appropriate, enable caching and compression.
- Enforcement: CI includes Lighthouse CI thresholds; builds fail if thresholds not met for main pages.

4) GDPR‑conscious enquiry handling
- Target behaviors:
  - Privacy page present and linked in footer before launch.
  - Contact form includes an explicit, unchecked consent checkbox with text: “I consent to Paceframe Coaching processing my personal data to respond to this enquiry. See Privacy policy.” Link to Privacy.
  - Only collect fields needed to respond; clearly state lawful basis (legitimate interest/contact request) in Privacy page.
  - Retention: default policy documented (e.g., delete enquiries after 12 months unless client opts in).
- Enforcement: CI/PR checklist requires a Privacy page stub and form consent control before merging to main.

5) Secure form processing
- Requirements:
  - Site served over HTTPS only (HSTS enabled in production).
  - Server-side validation for all form inputs; use anti‑forgery tokens (ASP.NET Core antiforgery).
  - Sanitize and encode outputs to avoid XSS; do not run user input as HTML.
  - Secrets (SendGrid API key, SMTP creds) stored in user-secrets/GitHub Secrets/Azure Key Vault; never committed.
  - Limit logging of PII: don’t store full form submissions in plaintext logs; redact or truncate sensitive fields.
  - Consider lightweight spam protection (honeypot field or rate-limiting). Optional CAPTCHA if spam is high.
- Enforcement: PR check verifies no secrets in repo; code review verifies antiforgery and server-side validation for contact endpoint.

6) Email delivery (SendGrid)
- Behavior:
  - Use SendGrid Web API via injected IEmailSender abstraction.
  - Retry and handle failures gracefully; on failure show friendly message to user and log failure for operator review.
  - Use verified sender address configured in production.
- Enforcement: unit tests mock IEmailSender; integration test with SendGrid sandbox key (manual) during staging.

7) SEO basics
- Requirements:
  - Page titles and meta descriptions per page; unique and descriptive.
  - One H1 per page, semantic heading structure.
  - Human-readable URLs (e.g., /services, /pricing).
  - Robots.txt and sitemap.xml generated and present in publish output.
  - Open Graph tags for sharing (title, description, image).
- Enforcement: simple SEO checklist in PR template; automated check for presence of title and meta description in templates.

8) Deployment & CI
- Pipeline (recommended GitHub Actions):
  - On PR: dotnet build, dotnet test, dotnet format check, accessibility (axe/pa11y), Lighthouse CI (Home, Contact), static scan for secrets.
  - On merge to main: build and publish to Azure App Service staging slot; require manual approval to swap to production.
- Secrets: use GitHub Secrets for CI; configure production secrets in Azure Key Vault.
- Enforcement: main branch protected; merge only on passing CI and 1 reviewer approval.

9) Testing & code quality
- Unit tests: critical code paths (contact form validation, email sender behavior) with minimum coverage for business logic (aim 60%+ for service layer).
- Static analysis: run dotnet format and prefer enabling Roslyn analyzers; treat analyzer warnings as build warnings—escalate to errors if chosen later.
- Enforcement: CI runs dotnet test; failures block merge.

10) Monitoring & incident response
- Instrumentation: Application Insights (or equivalent) for exceptions and request metrics.
- Alerts: notify on production 5xx spike, email-sending error rate above threshold, or health check failures.

11) Backups & data
- No database in v1. If storing enquiries later, require encrypted storage and backup policy with retention documented before collection begins.

ALWAYS / ASK FIRST / NEVER
- Always: run tests and accessibility checks before merging; keep secrets out of source; use HTTPS in all environments.
- Ask first: adding persistent storage for enquiries, adding user accounts, adding payment processing, changing data retention policy.
- Never: commit API keys or passwords; collect more personal data than needed; publish without a Privacy page.

SUCCESS CRITERIA (measurable)
1. CI passes on PRs with build, tests, accessibility and Lighthouse checks (or documented acceptable deviations).
2. Lighthouse Perf >= 80 and Accessibility >= 90 for Home and Contact in CI runs.
3. Privacy page exists and contact form includes explicit consent checkbox before production deploy.
4. SendGrid integration implemented behind config and not shipping keys in repo.

OPEN QUESTIONS
- Tolerance levels for Lighthouse failures on low-bandwidth mobile (CI emulation may be stricter than real users) — any adjustments?
- Will you want enquiry storage in a later sprint (admin view)? If yes, pick storage tech (Azure Table, SQL, or Blob).

---
Constraint owner: project maintainer. Review cadence: at feature milestones or quarterly.
