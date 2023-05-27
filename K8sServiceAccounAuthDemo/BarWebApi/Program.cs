using k8s;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port) && Int32.TryParse(port, out int portNumber))
{
    builder.WebHost.UseUrls($"http://*:{portNumber}");
}
// Add services to the container.

builder.Services.AddSingleton<Kubernetes>(sp =>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="T:k8s.KubernetesClientConfiguration" /> from default locations
    ///     If the KUBECONFIG environment variable is set, then that will be used.
    ///     Next, it looks for a config file at <see cref="F:k8s.KubernetesClientConfiguration.KubeConfigDefaultLocation" />.
    ///     Then, it checks whether it is executing inside a cluster and will use <see cref="M:k8s.KubernetesClientConfiguration.InClusterConfig" />.
    ///     Finally, if nothing else exists, it creates a default config with localhost:8080 as host.
    /// </summary>
    /// <remarks>
    ///     If multiple kubeconfig files are specified in the KUBECONFIG environment variable,
    ///     merges the files, where first occurrence wins. See https://kubernetes.io/docs/concepts/configuration/organize-cluster-access-kubeconfig/#merging-kubeconfig-files.
    /// </remarks>
    /// <returns>Instance of the<see cref="T:k8s.KubernetesClientConfiguration" /> class</returns>
    var config = KubernetesClientConfiguration.BuildDefaultConfig();
    return new Kubernetes(config);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var pathPrefix = builder.Configuration.GetValue<string>("pathPrefix");
var app = builder.Build();

if (!string.IsNullOrEmpty(pathPrefix))
{
    app.UsePathBase(pathPrefix);
}
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
