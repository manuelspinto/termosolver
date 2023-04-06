using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TermoSolver;
using TermoSolver.Services.Solver;
using TermoSolver.Services.Solver.Implementations;
using TermoSolver.Services.Solver.SolverStrategies;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IWordSolver, PepperoniSolver>();
builder.Services.AddScoped<ISolverService, SolverService>();
builder.Services.AddScoped<ISolverFactory, SolverFactory>();

/*builder.Services.AddDbContext<DictionaryContext>(
        options => options.UseSqlServer(builder.Configuration["ConnectionStrings:ConsumerAuto.ComplianceChecksDB"]));

*/
await builder.Build().RunAsync();
