# tasks-manager
Atualmente pensando no histórico de task, ele fará parte do agregado projects->task->history
Porém esse histórico pode ser visto como um evento de domínio, pensando em event source
Nesse caso pode fazer sentido no futuro esse histórico ser armazenado em um event store separado
Onde mesmo que uma task seja deletada, os eventos permanecem e o histórico poderia ser reconstruído a partir deles
Poderia ser pensar também em soft delete de projetos e tasks
Nesse caso a remoção de uma task seria uma alteração de estado, então também um evento.

Outra evolução futura é sobre comentários nas tasks
Atualmente comentários são apenas adicionados nas tasks, então pode ser interessante o contexto de comentário possui sua própria controller
Bem como seus próprios endpoints, similar ao modo que task se relaciona com project

Outro ponto de melhoria futura é o ajuste do contexto de banco de dados, pois atualmente, para quesitos de desenvolvimento
cada vez que o contexto é chamado é feita uma validação se o banco já existe, o que não é necessário.

Um ponto muito relevante é sobre autorização
Como a autenticação é um serviço externo que ainda não está integrado, pra verificar se o usuário possui autorização para obter
os dados na rota de relatório, está sendo feito pelo id passado na requisição

Visto que na geração de relatório há um período específico de buscar as tasks concluídas nos ultimos 30 dias
Seria mais organizado ter uma rota específica para atualização de status