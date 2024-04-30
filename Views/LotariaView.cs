using System;

public interface IModelObserver
{
    void UpdateView(); // Método para atualizar a View quando o modelo sofre alterações.
}

public class LotariaView : IModelObserver
{
    private ResultadoAposta _model; // Referência ao modelo para acesso às suas propriedades.

    // Construtor que aceita o modelo como parâmetro.
    public LotariaView(ResultadoAposta model)
    {
        _model = model;
    }

    // Método invocado quando ocorre uma mudança de estado no modelo.
    public void HandleStateChanged(object? sender, EventArgs e)
    {
        UpdateView(); // Atualiza a vista em resposta à mudança.
    }

    // Atualiza a View exibindo a chave sorteada e outras informações relevantes.
    public void UpdateView()
    {
        MostrarMensagem("A View foi notificada de uma mudança no Modelo!");
        // Se existirem números na chave sorteada, exibe-os.
        if (_model.ChaveSorteada != null && _model.ChaveSorteada.Length > 0)
        {
            MostrarMensagem($"Chave sorteada: {string.Join(", ", _model.ChaveSorteada)}");
        }
    }

    // Exibe uma mensagem na consola.
    public void MostrarMensagem(string mensagem)
    {
        Console.WriteLine(mensagem);
    }

    // Solicita entrada do utilizador e retorna uma string.
    public string SolicitarEntrada(string prompt)
    {
        Console.WriteLine(prompt); // Exibe o prompt para o utilizador.
        string input = Console.ReadLine()!; // Lê a entrada do utilizador.
        return input ?? string.Empty; // Retorna a entrada ou uma string vazia se for nula.
    }
}
