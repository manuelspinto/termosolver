using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TermoSolver;
using TermoSolver.Services.Solver;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IWordSolver, WordSolver>();

/*builder.Services.AddDbContext<DictionaryContext>(
        options => options.UseSqlServer(builder.Configuration["ConnectionStrings:ConsumerAuto.ComplianceChecksDB"]));

*/
await builder.Build().RunAsync();
