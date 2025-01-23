
## Informações

Essa aplicação é dividida em três partes a api aps.net, responsavel por fazer as chamadas para o ollama, e para uma outra api em python responsavel pelo embedding, a api python que usa all-MiniLM-L6-v2 para fazer o embedding de textos, o ollama rodando llama3.2 e pro ultimo o front ent para fazer a apresentação do chat

#### Biaxa o modelo base

```curl
  ollama pull llama3.2
```

#### Gera modelo personalizado baseado no llama3.2

```curl
  ollama create meu-modelo -f modelfile
```

