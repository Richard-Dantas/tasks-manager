# tasks-manager
Atualmente pensando no hist�rico de task, ele far� parte do agregado projects->task->history
Por�m esse hist�rico pode ser visto como um evento de dom�nio, pensando em event source
Nesse caso pode fazer sentido no futuro esse hist�rico ser armazenado em um event store separado
Onde mesmo que uma task seja deletada, os eventos permanecem e o hist�rico poderia ser reconstru�do a partir deles
Poderia ser pensar tamb�m em soft delete de projetos e tasks
Nesse caso a remo��o de uma task seria uma altera��o de estado, ent�o tamb�m um evento.