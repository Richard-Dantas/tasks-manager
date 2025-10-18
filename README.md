# tasks-manager
Atualmente pensando no hist�rico de task, ele far� parte do agregado projects->task->history
Por�m esse hist�rico pode ser visto como um evento de dom�nio, pensando em event source
Nesse caso pode fazer sentido no futuro esse hist�rico ser armazenado em um event store separado
Onde mesmo que uma task seja deletada, os eventos permanecem e o hist�rico poderia ser reconstru�do a partir deles
Poderia ser pensar tamb�m em soft delete de projetos e tasks
Nesse caso a remo��o de uma task seria uma altera��o de estado, ent�o tamb�m um evento.

Outra evolu��o futura � sobre coment�rios nas tasks
Atualmente coment�rios s�o apenas adicionados nas tasks, ent�o pode ser interessante o contexto de coment�rio possui sua pr�pria controller
Bem como seus pr�prios endpoints, similar ao modo que task se relaciona com project

Outro ponto de melhoria futura � o ajuste do contexto de banco de dados, pois atualmente, para quesitos de desenvolvimento
cada vez que o contexto � chamado � feita uma valida��o se o banco j� existe, o que n�o � necess�rio.

Um ponto muito relevante � sobre autoriza��o
Como a autentica��o � um servi�o externo que ainda n�o est� integrado, pra verificar se o usu�rio possui autoriza��o para obter
os dados na rota de relat�rio, est� sendo feito pelo id passado na requisi��o

Visto que na gera��o de relat�rio h� um per�odo espec�fico de buscar as tasks conclu�das nos ultimos 30 dias
Seria mais organizado ter uma rota espec�fica para atualiza��o de status