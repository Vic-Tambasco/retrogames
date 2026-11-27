namespace RetroGames.Model.Model;

public class ItemPedido
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
    public int IdPedido { get; set; }
    public int IdJogo { get; set; }
}