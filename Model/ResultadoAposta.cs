using System;
using System.Linq;
using System.Collections.Generic;

public class ResultadoAposta
{
    private event EventHandler? StateChanged; // Evento para notificar mudanças de estado.
    private HashSet<EventHandler> _observers = new HashSet<EventHandler>();

    private bool _stateChanged = false;
    private int[] _aposta = Array.Empty<int>();
    public int[] Aposta
    {
        get => _aposta;
        set
        {
            if (!_aposta.SequenceEqual(value))
            {
                _aposta = value;
                NotifyStateChanged();
            }
        }
    }

    private int[] _chaveSorteada = Array.Empty<int>();
    public int[] ChaveSorteada
    {
        get => _chaveSorteada;
        set
        {
            if (!_chaveSorteada.SequenceEqual(value))
            {
                _chaveSorteada = value;
                NotifyStateChanged();
            }
        }
    }

    private int _acertos = 0;
    public int Acertos
    {
        get => _acertos;
        set
        {
            if (_acertos != value)
            {
                _acertos = value;
                NotifyStateChanged();
            }
        }
    }

    private string _premio = string.Empty;
    public string Premio
    {
        get => _premio;
        set
        {
            if (_premio != value)
            {
                _premio = value;
                NotifyStateChanged();
            }
        }
    }

    public void AddObserver(EventHandler observer)
    {
        if (_observers.Add(observer))
        {
            StateChanged += observer;
        }
    }

    public void RemoveObserver(EventHandler observer)
    {
        if (_observers.Remove(observer))
        {
            StateChanged -= observer;
        }
    }

    protected void NotifyStateChanged()
    {
        if (_stateChanged)
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
            _stateChanged = false;
        }
    }

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
