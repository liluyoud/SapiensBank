using System.Text.Json;

public class Banco
{
    public List<Conta> Contas { get; set; } = new List<Conta>();

    public Banco()
    {
        GetContas();
    }

    public void GetContas()
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "SapiensBank", "banco.json");

        if (File.Exists(fullPath))
        {
            var json = File.ReadAllText(fullPath);

            Contas = JsonSerializer.Deserialize<List<Conta>>(json) ?? Contas;
        }
    } 

    public void SaveContas()
    {
        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "SapiensBank");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        var fullPath = Path.Combine(directoryPath, "banco.json");
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(Contas, options);
        File.WriteAllText(fullPath, json);
    }
}
