using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Lotaria.Logging;


public class LotariaController
{
    private LotariaView _view;
    private ResultadoAposta _model;
    private Random _rand = new Random();
    private IMessage _logger;  // Variável para o logger

    private const int N_BOLAS = 49;
    private const int N_CHAVE = 5;

    public LotariaController(LotariaView view, ResultadoAposta model, IMessage logger)
    {
        _view = view;
        _model = model;
        _logger = logger;

        _model.AddObserver(new EventHandler(_view.HandleStateChanged));
        _logger.Debug("LotariaController initialized.");
    }

    public void Iniciar()
    {
        bool continuar = true;
        _view.MostrarMensagem("Bem-vindo ao Programa Totoloto!");
        _logger.Debug("Programa iniciado.");
        while (continuar)
        {
            string opcao = _view.SolicitarEntrada("Escolha uma opção: A (Sortear), B (Aposta Simples), C/D (Aposta Múltipla), E (Consultar Última Aposta), F (Sair)");
            _logger.Debug($"Opção selecionada: {opcao}");
            try
            {
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
                        _logger.Debug("Encerrando o programa.");
                        break;
                    default:
                        _view.MostrarMensagem("Opção inválida.");
                        _logger.Warning("Opção inválida selecionada.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _view.MostrarMensagem($"Erro durante a operação: {ex.Message}");
                _logger.Error($"Exception: {ex.Message}");
            }
        }
    }

    private void Sortear()
    {
        _model.ChaveSorteada = Enumerable.Range(1, N_BOLAS).OrderBy(x => _rand.Next()).Take(N_CHAVE).ToArray();
        _logger.Debug($"Sorteio realizado: {string.Join(", ", _model.ChaveSorteada)}");
    }

    private void ApostaSimples()
    {
        bool apostaValida = false;
        while (!apostaValida)
        {
            string entrada = _view.SolicitarEntrada($"Indique a sua aposta [{N_CHAVE} números de 1 a {N_BOLAS}], separados por espaço:");
            _logger.Debug($"Entrada recebida para aposta simples: {entrada}");

            try
            {
                List<int> numerosEntrada = entrada.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                if (numerosEntrada.Count == N_CHAVE && numerosEntrada.All(n => n >= 1 && n <= N_BOLAS) && numerosEntrada.Distinct().Count() == N_CHAVE)
                {
                    _model.Aposta = numerosEntrada.ToArray();
                    apostaValida = true;
                    _logger.Debug($"Aposta validada e registrada: {string.Join(", ", numerosEntrada)}");
                }
                else
                {
                    _view.MostrarMensagem("Verifique sua aposta. Os números devem ser únicos e estar no intervalo de 1 a 49.");
                    _logger.Warning("Aposta inválida.");
                }
            }
            catch (FormatException)
            {
                _view.MostrarMensagem("Erro: As entradas devem ser todas numéricas.");
                _logger.Error("Erro de formatação nos números da aposta.");
            }
        }
    }

    private void ApostaMultipla(char tipoAposta)
    {
        var numeros = SolicitarApostaMultipla();
        if (tipoAposta == 'D' && numeros.Sum() > 100)
        {
            _view.MostrarMensagem("A soma dos números da aposta é maior do que 100, a aposta é especial!");
            _logger.Debug("Aposta especial realizada.");
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
                _logger.Warning("Dados de entrada inválidos para aposta múltipla.");
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
            _logger.Debug($"Combinação: {string.Join(", ", combinacao)} - Acertos: {acertos}");
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
        _view.MostrarMensagem($"Acertos: {_model.Acertos}, Prémio: {_model.Premio}");
        _logger.Debug($"Premiação calculada: Acertos: {_model.Acertos}, Prêmio: {_model.Premio}");
        SalvarResultadoEmJson();
    }

    private void SalvarResultadoEmJson()
    {
        string jsonString = JsonSerializer.Serialize(_model, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("resultadoAposta.json", jsonString);
        _logger.Debug("Resultado da aposta salvo em JSON.");
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
                _view.MostrarMensagem($"Prémio: {resultado.Premio ?? "Nenhum prémio"}");
                _logger.Debug("Última aposta consultada com sucesso.");
            }
            else
            {
                _view.MostrarMensagem("Não foi possível ler o resultado da última aposta.");
                _logger.Warning("Falha ao ler o resultado da última aposta.");
            }
        }
        catch (Exception ex)
        {
            _view.MostrarMensagem($"Erro ao consultar a última aposta: {ex.Message}");
            _logger.Error($"Erro ao consultar a última aposta: {ex.Message}");
        }
    }
}