# Perguntas, futuras implementações e melhorias
1 - Atualmente ao deletar o registro, ele é de fato removido do banco. Faz sentido em uma próxima sprint implementarmos um soft delete? Visto que temos um histórico para tasks e pode ser interessante manter esses registros no banco para o caso de uma necessidade de reconstruirmos uma determinada linha do tempo ou apenas ver todas as tarefas e projetos que já foram criados.

2 - O histórico de alteração de tarefas está apenas como uma snapshot daquela tarefa no momento da alteração. Isso por um lado é bom pois para cada alteração insere apenas um registro no banco, por outro lado eu não consigo rastrear o histórico de por exemplo as alterações de status. Faz sentido tratarmos o histórico como eventos de domínio e aplicarmos event source para que tenhamos rastreabilidade por atributo?

3 - Pela definição, comentários são apenas adicionados nas tasks, eles não podem ser editados e nem removidos. Faz sentido termos endpoints especificos para comentários e permitirmos alterações?

4 - Teremos envio de email para o gerente quando uma task for concluída, ou para um usuário comum quando uma task for atribuída a ele?

# O que pode ser melhorado no projeto
1 - Ter um catálogo de erros para retornos amigáveis ao usuário, criando uma classe estática e utilizando ErrorOr

2 - Integração com o sistema de autenticação para melhor o fluxo por exemplo de apenas gerentes serem autorizados a gerar relatórios, utilizando por exemplo Refit

3 - Separar rotas específicas para atualização de status e adição de comentários em tasks

4 - Implementar Cache utilizando o padrão decorator

5 - Pipelines de CI/CD

6 - Pensando em cloud, a estrutura já está bem receptiva para se utilizar de algumas coisas. Como melhoria seria interessante remover qualquer segredo que esteja dentro da solução que colocar por exemplo no Secrets Manager da AWS. Isso traria segurança pois mesmo com acesso ao repositório, o desenvolvedor precisaria se autenticar na AWS com o aws sso login depois de configurar o acesso na máquina, então ao executar a aplicação autenticado, ela obteria os segredos automaticamente.

7 - Listagens paginadas

# Como executar
1 - Clone o projeto

2 - Abra a solução no Visual Studio

3 - Clique em "Docker Compose" para executar a aplicação

4 - Abra https://localhost:8001/swagger/index.html

Ao subir a aplicação, serão criados 3 usuários automaticamente, um deles possui a role de gerente, com o id "33333333-3333-3333-3333-333333333333" para geração de relatórios. Os outros dois seguem o mesmo padrão, um deles com id "11111111-1111-1111-1111-111111111111"

# Cobertura de testes
```
dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet test .\Tasks.Manager.sln --collect:"XPlat Code Coverage"
reportgenerator -reports:"'seu_caminho'\Task.Manager.Test\TestResults\*\coverage.cobertura.xml" -targetdir:"E:\testes-tecnicos\coveragereport" -reporttypes:Html
```

<img width="766" height="206" alt="image" src="https://github.com/user-attachments/assets/5da97a94-5e4f-4aac-9e71-cd4b3ea7ca43" />

<img width="1623" height="665" alt="image" src="https://github.com/user-attachments/assets/944a3123-8eee-49d8-8f54-b23c5839e1be" />

# Banco de Dados
<img width="566" height="435" alt="image" src="https://github.com/user-attachments/assets/17ba82b7-fb23-448d-8c15-f30fc0980127" />

# Commits
<img width="1326" height="832" alt="image" src="https://github.com/user-attachments/assets/5499f420-1e7a-484c-a317-fcadb8985e52" />


# Linhas de raciocínio anotadas durante o desenvolvimento
A estrutura de pastas foi separada assim:

<img width="290" height="192" alt="image" src="https://github.com/user-attachments/assets/029240cf-02eb-4b8a-849e-26d2acdec5eb" />

Caso haja a necessidade de novos APIs, como por exemplo uma API Operacional no futuro, ela pode ser adicionada e usufruir do domínio construído

Domínios ricos
Agregados: projects->task->history/Comments
Repository Pattern
Clean architecture


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

TODOS OS REQUISITOS DO TESTE TÉCNICO CONCLUÍDOS
