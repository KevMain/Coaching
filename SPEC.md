# Spec: Paceframe Coaching Website (First release)

## Objective
Build a professional, responsive website for Paceframe Coaching that establishes credibility, explains coaching services, shows approach and experience, captures enquiries, and makes it easy to book an initial (free) discovery call.

Primary user: beginner-to-intermediate runners (age ~30–40+, any gender) who started running later in life, based locally, often time-poor and willing to pay for personalised coaching to increase speed safely.

Success looks like:
- Clear presentation of 1:1 coaching offering and pricing
- A short discovery booking flow (CTA → contact form + calendar placeholder)
- Contact/enquiry form that reliably sends notifications to kevmain@gmail.com via SendGrid
- Clean brand look (teal-led palette, Inter font, strong wordmark)
- Deployable to Azure App Service at paceframecoaching.co.uk (provisional)

## Assumptions
1. This will be an ASP.NET Core app targeting .NET 10 with server-rendered pages (Razor Pages or MVC views).
2. Email delivery will be via SendGrid; the SendGrid API key and sender address will be provided later via configuration.
3. No payment or subscription automation in v1; pricing is informational and onboarding/payment handled manually.
4. Calendar is a placeholder embed area (customer will plug Calendly / Microsoft Bookings later).
5. No persistent database required for v1 — contact form sends email only. If storage is needed later, a follow-up change will add it.

## Tech stack
- .NET 10, ASP.NET Core (Razor Pages preferred)
- Frontend: server-rendered HTML, CSS (Tailwind or plain CSS; choose one in implementation step), Inter font
- Email: SendGrid API integration
- Hosting: Azure App Service (build & deployment scripts included)

## Commands
- Build: dotnet build
- Run locally: dotnet run --project src/Paceframe.Web
- Test: dotnet test
- Publish (Azure-ready): dotnet publish -c Release -o ./publish

## Project structure
- src/Paceframe.Web/         → ASP.NET Core web app (pages, controllers, views)
  - Pages/ or Controllers+Views/ → Home, About, Services, Pricing, Approach, Contact
  - Shared/ → _Layout, partials
  - wwwroot/ → static assets (css, images, fonts)
- tests/                     → unit/feature tests (minimal for controllers/pages)
- docs/                      → deployment and content notes

## Pages (required)
- Home — hero (headline, subheading, CTA), overview of services, short bio, testimonials placeholder, CTA
- About — full bio, credentials (placeholders), coaching philosophy, headshot
- Services — details of 1:1 coaching (primary), short notes about other options (placeholder / contact for quote)
- Pricing — clear display: 1:1 coaching £90/month; other services: contact for quote
- Approach — explain training methodology, communication cadence, examples of what a client receives
- Contact — enquiry form (fields listed below) and calendar embed placeholder

Optional (defer to later): Testimonials, FAQ, Blog, Terms & Privacy pages (privacy page should be added before public launch).

## Contact / enquiry form
Fields (required by spec):
- Name
- Email
- Phone
- Location
- Age
- Running experience (free text)
- Current weekly mileage
- Goal (free text)
- Preferred contact method
- Availability / best times

Behavior:
- Validate required fields client- and server-side
- Send an email via SendGrid to kevmain@gmail.com with the submission details
- Show a confirmation page / inline success message after submit
- No persistent storage in v1 (Store option deferred). If storing submissions is later requested, add a secure storage backing (Azure Table/SQL)

## Booking flow
- Primary CTA: "Book a free discovery call" — links to Contact page with the calendar embed placeholder above the form.
- The calendar area is an embeddable placeholder (HTML container) so Calendly or another provider can be added later.

## Brand & Design
- Business name: Paceframe Coaching
- Tagline: "Smarter training. Stronger running. Your goals, your pace."
- Hero content (from user):
  - Headline: Train smarter. Run stronger.
  - Subheading: Personalised running coaching for runners who want clear, sustainable progress — whether you’re preparing for your first event, chasing a new personal best, or simply ready to enjoy your running more.
  - Primary CTA: Book a free discovery call
  - Hero image preference: natural outdoor action shot of Kevin running on a road or trail

Brand palette (Option A, refined):
- Primary teal: #0B6E62
- Accent orange (use sparingly): #FF7A3D (use darker variant for CTA background if needed)
- Background light: #F5F7F9
- Dark text: #1F2933
- Font: Inter (weights 400/500 for body, 700 for headings)

Logo: strong wordmark "Paceframe Coaching" with a subtle frame/pace marker detail in the a/f of "Paceframe" (implemented as SVG placeholder).

Photos: use stock action shots for v1 and include a headshot placeholder.

## Code style
- Follow standard C# conventions (PascalCase for types, camelCase for private fields, async suffix for async methods).
- Keep controllers/pages thin; view models for page data.
- Use dependency injection for SendGrid client and configuration.

Example snippet (style reference):
```csharp
// ...existing code...
public class ContactModel : PageModel
{
	private readonly IEmailSender _email;
	public ContactModel(IEmailSender email) { _email = email; }
	public async Task<IActionResult> OnPostAsync(ContactForm form)
	{
		if (!ModelState.IsValid) return Page();
		await _email.SendContactAsync("kevmain@gmail.com", form);
		return RedirectToPage("Thanks");
	}
}
```

## Testing strategy
- Unit tests for: contact form validation, email service abstraction (mock SendGrid), and basic page rendering.
- Manual UI testing checklist: responsive layout, form submit success, SendGrid integration with a test API key.

## Boundaries
- Always: validate inputs server-side, keep secrets out of source (use user secrets / Azure Key Vault), include Privacy page before public launch.
- Ask first: any database or persistent storage additions, any change to the hosting target, adding payment integration.
- Never: commit real SendGrid API keys or other secrets to repo, remove required accessibility features (contrast, alt text).

## Success criteria (testable)
1. All required pages (Home, About, Services, Pricing, Approach, Contact) exist and render responsively.
2. Home hero matches provided content and brand palette is applied.
3. Contact form validates inputs and sends an email to kevmain@gmail.com using a configurable SendGrid API key.
4. Calendar embed placeholder is present on Contact page and documented how to replace with Calendly.
5. Instructions or script are provided for deploying to Azure App Service and setting SendGrid configuration.

## Open questions / follow-ups
1. Would you like submissions also stored on the site (admin view) in a later sprint? (default: no)
2. Do you want a Privacy policy drafted for the site or will you provide one? (required before launch)
3. When ready to publish, provide SendGrid sender address and API key, and Azure publish target credentials.
4. Do you want any analytics (Google Analytics/GA4) included in v1?

---
Spec author: automated spec generator (interview completed) — implement when approved.
