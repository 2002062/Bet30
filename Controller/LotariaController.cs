using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class LotariaController
{
    private LotariaView _view;
    private ResultadoAposta _model;
    private Random _rand = new Random();
    private const int N_BOLAS = 49;
    private const int N_CHAVE = 5;

    public LotariaController(LotariaView view, ResultadoAposta model)
    {
        _view = view;
        _model = model;
        _model.AddObserver(new EventHandler(_view.HandleStateChanged));
    }

    public void Iniciar()
    {
        bool continuar = true;
        _view.MostrarMensagem("Bem-vindo ao Programa Totoloto!");
        while (continuar)
        {
            string opcao = _view.SolicitarEntrada("Escolha uma opção: A (Sortear), B (Aposta Simples), C/D (Aposta Múltipla), E (Consultar Última Aposta), F (Sair)");
            switch (opcao.ToUpper())
            {
                case "A":
                    Sortear();
                    _view.UpdateView();
                    break;
                case "B":
                    ApostaSimples();
                    Sortear();
                    ValidaPremioAposta();
                    break;
                case "C":
                case "D":
                    ApostaMultipla(opcao[0]);
                    break;
                case "E":
                    ConsultarUltimaAposta();
                    break;
                case "F":
                    continuar = false;
                    break;
                default:
                    _view.MostrarMensagem("Opção inválida.");
                    break;
            }
        }
    }

    private void Sortear()
    {
        _model.ChaveSorteada = Enumerable.Range(1, N_BOLAS).OrderBy(x => _rand.Next()).Take(N_CHAVE).ToArray();
    }

private void ApostaSimples()
{
    bool apostaValida = false;
    while (!apostaValida)
    {
        string entrada = _view.SolicitarEntrada($"Indique a sua aposta [{N_CHAVE} números de 1 a {N_BOLAS}], separados por espaço:");
        var entradas = entrada.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        List<int> numerosEntrada;
        try
        {
            numerosEntrada = entradas.Select(int.Parse).ToList();
        }
        catch (FormatException)
        {
            _view.MostrarMensagem("Erro: As entradas devem ser todas numéricas.");
            continue;
        }

        if (numerosEntrada.Count != N_CHAVE || numerosEntrada.Any(n => n < 1 || n > N_BOLAS) || numerosEntrada.Distinct().Count() != N_CHAVE)
        {
            _view.MostrarMensagem("Verifique sua aposta. Os números devem ser únicos e estar no intervalo de 1 a 49.");
            continue;
        }

        _model.Aposta = numerosEntrada.ToArray();
        apostaValida = true;
    }
}

    private void ApostaMultipla(char tipoAposta)
    {
        var numeros = SolicitarApostaMultipla();
        if (tipoAposta == 'D' && numeros.Sum() > 100)
        {
            _view.MostrarMensagem("A soma dos números da aposta é maior do que 100, a aposta é especial!");
        }
        Sortear();
        GerarECompararCombinacoes(numeros);
    }

    private HashSet<int> SolicitarApostaMultipla()
    {
        HashSet<int> numeros;
        do
        {
            string entrada = _view.SolicitarEntrada($"Introduza os números da sua aposta, separados por espaço (mais de {N_CHAVE} números, de 1 a {N_BOLAS}):");
            numeros = new HashSet<int>(entrada.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            if (numeros.Count <= N_CHAVE || numeros.Any(n => n < 1 || n > N_BOLAS))
            {
                _view.MostrarMensagem($"Todos os números devem estar no intervalo de 1 a {N_BOLAS} e deve introduzir mais de {N_CHAVE} números.");
            }
        } while (numeros.Count <= N_CHAVE || numeros.Any(n => n < 1 || n > N_BOLAS));

        return numeros;
    }

    private void GerarECompararCombinacoes(HashSet<int> numerosAposta)
    {
        List<int> numerosList = numerosAposta.ToList();
        List<List<int>> todasCombinacoes = new List<List<int>>();
        GerarCombinacoes(numerosList, 0, N_CHAVE, new List<int>(), todasCombinacoes);
        foreach (var combinacao in todasCombinacoes)
        {
            int acertos = combinacao.Intersect(_model.ChaveSorteada).Count();
            _view.MostrarMensagem($"Combinação: {string.Join(", ", combinacao)} - Acertos: {acertos}");
        }
    }

    private void GerarCombinacoes(List<int> numeros, int inicio, int profundidade, List<int> combinacaoAtual, List<List<int>> todasCombinacoes)
    {
        if (profundidade == 0)
        {
            todasCombinacoes.Add(new List<int>(combinacaoAtual));
            return;
        }
        for (int i = inicio; i <= numeros.Count - profundidade; i++)
        {
            combinacaoAtual.Add(numeros[i]);
            GerarCombinacoes(numeros, i + 1, profundidade - 1, combinacaoAtual, todasCombinacoes);
            combinacaoAtual.RemoveAt(combinacaoAtual.Count - 1);
        }
    }

    private void ValidaPremioAposta()
    {
        _model.Premio = ResultadoAposta.DeterminarPremio(_model.Acertos);
        _view.MostrarMensagem($"Acertos: {_model.Acertos}, Prêmio: {_model.Premio}");
        SalvarResultadoEmJson();
    }

    private void SalvarResultadoEmJson()
    {
        string jsonString = JsonSerializer.Serialize(_model, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("resultadoAposta.json", jsonString);
    }

    private void ConsultarUltimaAposta()
    {
        try
        {
            string jsonString = File.ReadAllText("resultadoAposta.json");
            ResultadoAposta? resultado = JsonSerializer.Deserialize<ResultadoAposta>(jsonString);

            if (resultado != null)
            {
                _view.MostrarMensagem($"Chave sorteada: {string.Join(", ", resultado.ChaveSorteada ?? Array.Empty<int>())}");
                _view.MostrarMensagem($"Sua aposta: {string.Join(", ", resultado.Aposta ?? Array.Empty<int>())}");
                _view.MostrarMensagem($"Acertos: {resultado.Acertos}");
                _view.MostrarMensagem($"Prêmio: {resultado.Premio ?? "Nenhum prêmio"}");
            }
            else
            {
                _view.MostrarMensagem("Não foi possível ler o resultado da última aposta.");
            }
        }
        catch (Exception ex)
        {
            _view.MostrarMensagem($"Erro ao consultar a última aposta: {ex.Message}");
        }
    }
}
