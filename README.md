UAB - Laboratório de Software - Projeto de Lotaria

O projeto foi desenvolvido em C# e seguindo o padrão de arquitetura MVC (Model-View-Controller), que simula um sistema onde utilizadores podem participar em sorteios, realizar apostas e consultar resultados anteriores.

Componentes do Sistema:

    Programa_Totoloto.cs: Módulo inicial que configura e inicia a interação com o utilizador.
    LotariaController.cs: Responsável por gerir a lógica do programa, processando as entradas dos utilizadores e coordenando a interação entre a vista e o modelo.
    ResultadoAposta.cs: Armazena os dados das apostas, chaves sorteadas e os resultados, além de calcular os prémios.
    LotariaView.cs: Gerencia a interface de utilizador, exibindo mensagens e recolhendo entradas.

Funcionalidades Disponíveis:

    A (Sortear): Executa um sorteio de números.
    B (Aposta Simples): Permite ao utilizador apostar com 5 números.
    C/D (Aposta Múltipla): Permite apostas com mais de 5 números, com a opção 'D' oferecendo regras adicionais.
    E (Consultar Última Aposta): Facilita a consulta dos detalhes de apostas anteriores.

Detalhes Técnicos e Implementações:

    Model (ResultadoAposta): Centraliza a gestão dos dados e lógica para determinação de prémios, utilizando eventos para notificar a vista de mudanças no estado.
    View (LotariaView): Reage às notificações do modelo para atualizar a interface do utilizador, mantendo-se como um componente passivo que apenas responde às mudanças.
    Controller (LotariaController): Funciona como o intermediário, solicitando ações do modelo e atualizando a vista com base nos resultados obtidos.

Melhorias na Comunicação e Redução de Acoplamento:
Implementado os eventos e delegados para facilitar a comunicação entre o modelo e a vista, permitindo que o modelo notifique a vista sobre qualquer alteração sem intermediação direta do controlador. 
Esta estratégia reduziu significativamente o acoplamento entre os componentes, permitindo maior flexibilidade e facilidade na manutenção e expansão do sistema.

Considerações Finais:
O uso de MVC como arquitetura, juntamente com a implementação de eventos e delegados para comunicação entre os componentes, provou ser uma abordagem eficaz para o desenvolvimento deste sistema de lotaria. 
As práticas adotadas não só facilitaram a gestão e expansão do código, como também melhoraram a interatividade e a experiência do utilizador. 
Estamos abertos a feedbacks e sugestões para futuras melhorias e esperamos que as funcionalidades implementadas atendam às expectativas dos utilizadores.
