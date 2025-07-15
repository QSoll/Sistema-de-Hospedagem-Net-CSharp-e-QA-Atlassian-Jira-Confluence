# 🌐 Fluxo Operacional – Sistema de Hospedagem

Este sistema foi desenvolvido para auxiliar o gerenciamento de clientes, reservas e espaços de hospedagem, seguindo uma lógica simples, porém flexível e poderosa.

---

## 🧭 Linha do Tempo Operacional

| Etapa | Acontecimento                             | Requisitos                | Resultado                                                |
|-------|--------------------------------------------|---------------------------|----------------------------------------------------------|
| 1️⃣    | Cadastro de espaços (`EspacoHospedagem`)   | Nenhum                    | Estabelecimento fica apto a receber reservas             |
| 2️⃣    | Cadastro de cliente (`Cliente`)            | Ocorre na 1ª reserva      | Cliente passa a existir no sistema                       |
| 3️⃣    | Criação de reserva (`Reserva`)             | Pelo menos 1 espaço       | Espaço é ocupado, cliente vira hóspede                   |
| 4️⃣    | Prolongamento da reserva (opcional)        | Reserva ativa             | `DiasReservados` é atualizado e afeta valor total        |
| 5️⃣    | Encerramento de reserva (Check-out)        | Reserva ativa             | Reserva é desativada, espaço liberado, cliente deixa de ser hóspede |

---

## 🔄 Regras e Conceitos de Domínio

| Entidade      | Definição                        | Estado dinâmico               | Observações                                               |
|---------------|----------------------------------|-------------------------------|-----------------------------------------------------------|
| **Cliente**   | Pessoa cadastrada                | Pode ou não estar hospedado   | Criado na 1ª hospedagem. É persistente.                   |
| **Reserva**   | Associação entre cliente e espaço| Ativa ou encerrada            | Encerramento libera o espaço                              |
| **Hóspede**   | Cliente com reserva ativa        | Deixa de ser ao fazer checkout| Papel temporário — sem classe separada                    |
| **Espaço**    | Unidade de hospedagem            | Ocupado ou disponível         | Disponibilidade controlada via reservas ativas           |

---

## 📌 Regras Técnicas do Sistema

- Nenhuma reserva pode ser criada sem ao menos **um espaço cadastrado**
- A hospedagem exige vínculo de **pelo menos um cliente ativo**
- **Extensões de reserva** devem atualizar `DiasReservados` e recalcular valor total
- Após o checkout, o espaço é **automaticamente liberado** para nova reserva
- O cliente é **cadastrado no momento de sua primeira hospedagem**

---

> Esse modelo garante integridade de dados, clareza no fluxo de operação e uma experiência descomplicada tanto para operadores quanto para usuários do sistema.
