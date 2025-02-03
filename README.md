# Validador de CPF com Azure Functions

Este projeto é um microsserviço serverless desenvolvido com **Azure Functions** para validar números de CPF.  
Ele verifica se um CPF é válido de acordo com as regras oficiais e também simula consultas a bases de dados de fraudes e débitos para fornecer uma resposta completa.

## Funcionalidades

### Validação de CPF
- Verifica se o CPF é válido (incluindo dígitos verificadores).
- Rejeita CPFs inválidos, como sequências de números repetidos (ex: `00000000000`).

### Simulação de Consultas
- Verifica se o CPF consta em uma base de dados de fraudes.
- Verifica se o CPF consta em uma base de dados de débitos.

### Resposta Detalhada
- Retorna uma mensagem clara indicando se o CPF é válido e se há problemas associados (fraudes ou débitos).

## Tecnologias Utilizadas
- **Azure Functions:** Para hospedar o microsserviço serverless.
- **C# (.NET):** Para a lógica de validação e processamento.
- **GitHub:** Para versionamento e colaboração.

## Como Usar

### Clone o repositório:
```bash
git clone https://github.com/Dev-LeandroCoelho/httpValidaCpf.git
```

### Execute a função localmente:
```bash
func start
```

### Envie uma requisição `POST` para `http://localhost:7190/api/fnvalidacpf` com o CPF no corpo da requisição:
```json
{
    "cpf": "123.456.789-09"
}
```

### A função retornará uma resposta no formato JSON:
```json
{
    "cpf": "12345678909",
    "isValid": true,
    "message": "CPF válido, não consta na base de dados de fraudes e não consta na base de débitos."
}
```

## Próximos Passos
- Integrar com bases de dados reais de fraudes e débitos.
- Adicionar autenticação e autorização para proteger o endpoint.
- Publicar a função no Azure para uso em produção.

## Contribuições
Contribuições são bem-vindas!  
Sinta-se à vontade para abrir *issues* ou enviar *pull requests*.

## Licença
Este projeto está licenciado sob a **MIT License**.
