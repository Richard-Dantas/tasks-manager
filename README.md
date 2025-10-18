# tasks-manager
Atualmente pensando no histórico de task, ele fará parte do agregado projects->task->history
Porém esse histórico pode ser visto como um evento de domínio, pensando em event source
Nesse caso pode fazer sentido no futuro esse histórico ser armazenado em um event store separado
Onde mesmo que uma task seja deletada, os eventos permanecem e o histórico poderia ser reconstruído a partir deles
Poderia ser pensar também em soft delete de projetos e tasks
Nesse caso a remoção de uma task seria uma alteração de estado, então também um evento.