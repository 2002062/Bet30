using System;
using System.Linq;
using System.Collections.Generic;

public class ResultadoAposta
{
    private event EventHandler? StateChanged; // Evento para notificar mudanças de estado.
    private HashSet<EventHandler> _observers = new HashSet<EventHandler>(); // Conjunto de observadores que são notificados das mudanças.

    private bool _stateChanged = false; // Indicador para controlar se ocorreu uma mudança de estado.

    private int[] _aposta = Array.Empty<int>(); // Array que guarda a aposta atual do utilizador.
    public int[] Aposta
    {
        get => _aposta;
        set
        {
            // Verifica se a nova aposta é diferente da atual antes de atualizar.
            if (!_aposta.SequenceEqual(value))
            {
                _aposta = value;
                _stateChanged = true; // Marca que o estado mudou.
            }
        }
    }

    private int[] _chaveSorteada = Array.Empty<int>(); // Array que guarda a chave sorteada.
    public int[] ChaveSorteada
    {
        get => _chaveSorteada;
        set
        {
            // Verifica se a nova chave sorteada é diferente da atual antes de atualizar.
            if (!_chaveSorteada.SequenceEqual(value))
            {
                _chaveSorteada = value;
                _stateChanged = true; // Marca que o estado mudou.
            }
        }
    }

    private int _acertos = 0; // Número de acertos da última aposta.
    public int Acertos
    {
        get => _acertos;
        set
        {
            // Atualiza o número de acertos apenas se for diferente do valor atual.
            if (_acertos != value)
            {
                _acertos = value;
                _stateChanged = true; // Marca que o estado mudou.
            }
        }
    }

    private string _premio = string.Empty; // Descrição do prémio ganho.
    public string Premio
    {
        get => _premio;
        set
        {
            // Atualiza o prémio apenas se for diferente do valor atual.
            if (_premio != value)
            {
                _premio = value;
                _stateChanged = true; // Marca que o estado mudou.
            }
        }
    }

    // Adiciona um observador ao conjunto de observadores e ao evento de mudança de estado.
    public void AddObserver(EventHandler observer)
    {
        if (_observers.Add(observer))
        {
            StateChanged += observer;
        }
    }

    // Remove um observador do conjunto de observadores e do evento de mudança de estado.
    public void RemoveObserver(EventHandler observer)
    {
        if (_observers.Remove(observer))
        {
            StateChanged -= observer;
        }
    }

    // Invoca o evento de mudança de estado se houve alguma mudança.
    protected virtual void OnStateChanged()
    {
        if (_stateChanged)
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
            _stateChanged = false; // Reinicializa o indicador de mudança de estado.
        }
    }

    // Método estático para determinar o prémio com base no número de acertos.
    public static string DeterminarPremio(int quantidadeDeAcertos)
    {
        switch (quantidadeDeAcertos)
        {
            case 5: return "Parabéns! Ganhou o prémio máximo de 100,000 euros!";
            case 4: return "Excelente! Ganhou o segundo prémio de 10,000 euros!";
            case 3: return "Bom trabalho, ganhou um prémio de 1,000 euros!";
            case 2: return "Acertou dois números. Ganhou uma aposta grátis!";
            default: return "Que pena! Não foi desta vez. Tente novamente.";
        }
    }
}
