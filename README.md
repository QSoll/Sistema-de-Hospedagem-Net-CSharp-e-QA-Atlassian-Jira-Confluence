## Sistema de Gerenciamento de Hospedagem

Um sistema de terminal em C# para gerenciar espaços, hóspedes, reservas e emissão de comprovantes.  
Projetado com foco em **resiliência de entrada**, **experiência do usuário no console**, **modularidade** e **visual padronizado com cores**.

Ideal para hotéis, pousadas e centros de hospedagem.


trilha-net-explorando-desafio/
│
├── 📁 Comprovantes/.................# Pasta onde são salvos arquivos .txt de comprovantes
│   └── Comprovante_*.txt ...........# Arquivo gerado automaticamente com base na data
│
├── 📁 Contexto/ 
│   └── SessaoAtual.cs ..............# Gerencia reservas, clientes e seleção ativa da sessão
│
├── 📁 Controladoras/................# Camada de lógica que orquestra os dados
│   ├── ClienteControlador.cs........# Métodos: Cadastrar, Excluir, Listar clientes
│   ├── EspacoControlador.cs.........# Métodos: Cadastrar, Editar, Exibir, Excluir, Listar espaços
│   └── ReservaControladora.cs.......# Método principal: CriarReserva
│
├── 📁 Menus/.......................# Interface de navegação para cada módulo funcional
│   ├── MenuPrincipal.cs ............# Ponto central de entrada – exibe módulos principais
│   ├── MenuEspacos.cs ..............# Gerencia espaços de hospedagem
│   ├── MenuHospedes.cs..............# Gerencia hóspedes e suas ações
│   ├── MenuReservas.cs..............# Cria, finaliza, cancela e vincula reservas
│   ├── MenuComprovantes.cs..........# Emite comprovantes de reserva – visual ou arquivo
│   ├── MenuRelatorios.cs ...........# Gera relatórios: financeiro e clientes únicos
│   └── MenuSobreSistema.cs .........# Tela institucional sobre o projeto
│
├── 📁 Models/.......................# Classes que representam os dados
│   ├── Clientes.cs .................# Propriedades: Nome, Sobrenome, Email, CPF, etc.
│   └── Reserva.cs...................# Propriedades e métodos: Check-in, cálculo, clientes vinculados
│
├── 📁 Servicos/
│   └── ServicoClientes.cs ...........# Responsável por salvar dados dos clientes em arquivo
│
├── 📁 Utils/ ........................# Funções auxiliares reutilizáveis e visuais
│   ├── CentralDeNavegacao.cs.........# Navegação segura, encerramento e reinício
│   ├── MiniMenuHelper.cs.............# Rodapé interativo com atalhos
│   ├── CentralEntrada.cs.............# Entrada validada e contextual
│   ├── CentralEntradaInteira.cs......# Entrada de inteiros com limite e datas
│   ├── CentralEntradaControladora.cs.# Entrada de email, texto, números com escape
│   ├── CentralSaida.cs...............# Saída estilizada com cores e bordas
│   ├── EntradaHelper.cs..............# Confirmações, retorno visual
│   ├── InterfaceHelper.cs............# Animações e escrita com estilo
│   └── MiniMenu.cs...................# Componente de rodapé interativo
│
├── DesafioProjetoHospedagem.csproj ..# Arquivo do projeto .NET
├── Program.cs........................# Método Main com menu principal
├── README.md ........................# Documentação do projeto
└── trilha-net-explorando-desafio.sl..# Solução Visual Studio

## Entrada Segura (`EntradaHelper.cs`)

- Cancelamento com Enter repetido
- Validações inteligentes de texto, números e e-mails
- Confirmação Sim/Não padronizada
- Nome sugerido com override
- Nenhum `ReadLine()` fora do helper

---

## Comprovantes

- Nome do(s) hóspede(s), espaço, valor, período
- Visual com bordas, alinhamento e cores
- Salvo automaticamente com confirmação
- Emissão condicionada à existência de reserva ativa

- Opções de:
  - Visualizar na tela com borda e cores
  - Salvar como `.txt` com codificação UTF-8 sem BOM
- Mensagem explicativa para testadores sobre o funcionamento do recurso
- Navegação final via rodapé com destino manual

---

## Menu Rodapé Dinâmico

✅ Entrada independente e fixa para rodapé

✅ Sem null, sem escape silencioso

✅ Mensagens emocionais suaves e escalonadas

✅ Centralização visual e coerência com o projeto inteiro

✅ Respeita a navegação reversa (Voltar) e principal (MenuPrincipal)

✅ Sai apenas com 0, nunca por ENTER solto

---

### Cadastro
- Espaços
- Clientes
- Reservas

### Listagem
- Espaços com status de ocupação
- Reservas por status
- Hóspedes por nome parcial

### Cancelamento e Exclusões
- Exclusões protegidas por confirmação
- Espaços não excluídos se estiverem ocupados

### Validação de Erros
- Entradas inválidas tratadas com mensagens claras
- Comportamento resiliente e sem travamentos

---

## Cores no Console

- 🔵 Azul: perguntas
- 🟥 Vermelho: erros
- ✅ Verde: sucesso
- ⚪ Branco: textos neutros

---

## Sobre o sistema
Sistema desenvolvido com foco no terminal intuitivo e acessível.  
A navegação por atalhos e mensagens coloridas facilita o uso mesmo para iniciantes.  
Todas as operações possuem validação, confirmação ou retorno visual.
---

## Conclusão

Sistema pronto para produção:

- ✅ Modular e organizado
- ✅ Interface amigável
- ✅ Fluxos seguros
- ✅ Estilização uniforme
- ✅ Operações com atalhos inteligentes

---

> Desenvolvido por <a href="https://solmorcillo.com.br" title="Website Sol Morcillo" target="_blank"><img src="logo_SM.jpg" width="40" height="50"></a> 💛 com atenção aos detalhes
  Pensando na experiência do usuário.

## ▶️ Como executar

1. Clone o repositório:  
   `git clone https://github.com/QSoll/SistemaHospedagem.git`

2. Abra o projeto no Visual Studio ou VS Code

3. Compile e execute:  
   `dotnet run`

4. Navegue pelo menu principal e explore os módulos

<a href="https://solmorcillo.com.br" title="Website Sol Morcillo" target="_blank"><img src="logo_SM.jpg" width="60" height="70"></a>


