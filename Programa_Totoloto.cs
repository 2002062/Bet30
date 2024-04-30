using System;

class Program
{
    static void Main(string[] args)
    {
        // Criação do modelo de dados para a aplicação.
        ResultadoAposta model = new ResultadoAposta();
        
        // Criação da interface de utilizador, passando o modelo como referência para que possa observar mudanças.
        LotariaView view = new LotariaView(model);
        
        // Criação do controlador que gerencia a lógica de interação entre a view e o modelo.
        LotariaController controller = new LotariaController(view, model);

        // Registra a view como um observador do modelo para que seja notificada sobre mudanças de estado.
        model.AddObserver(view.HandleStateChanged);

        // Inicia o processo de interação com o utilizador, começando o ciclo de vida da aplicação.
        controller.Iniciar();
    }
}
