using System;
using Lotaria.Logging;

class Program
{
    static void Main(string[] args)
    {
        // Criação da instância 'model' do tipo ResultadoAposta. Este objeto será responsável por armazenar e gerir o estado dos dados da aposta.
        ResultadoAposta model = new ResultadoAposta();
        
        // Criação da instância 'view' do tipo LotariaView. Esta classe é a interface de utilizador que irá interagir com o utilizador final.
        LotariaView view = new LotariaView(model);

        // Criação do logger que grava as mensagens na console e em um arquivo JSON.
        IMessage logger = new ConsoleAndJsonLogger("lotariaLog.json");
        
        // Criação da instância 'controller' do tipo LotariaController. Este controlador faz a mediação da interação entre o modelo (model) e a vista (view), usando o logger para registrar atividades.
        LotariaController controller = new LotariaController(view, model, logger);

        // Registo da 'view' como um observador do 'model'. Isto significa que sempre que o estado do 'model' for alterado, a 'view' será notificada para que possa atualizar a apresentação ao utilizador.
        model.AddObserver(view.HandleStateChanged);

        // Início do processo de interação com o utilizador, ativando o ciclo de vida principal da aplicação de lotaria.
        controller.Iniciar();
    }
}