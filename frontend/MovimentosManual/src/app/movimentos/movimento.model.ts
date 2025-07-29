export interface MovimentoManual {
  mes: string;                     // Mês do movimento como texto
  ano: string;                     // Ano do movimento como texto
  numeroLancamento: number;       // Identificador único do lançamento
  codigoProduto: string;          // Código do produto
  codigoCosif: string;            // Código COSIF associado ao produto
  descricao: string;              // Descrição do lançamento
  valor: number;                  // Valor monetário
  dataMovimento?: string;         // Data opcional do movimento (formato ISO 8601)
  codigoUsuario: string;          // Usuário responsável pelo lançamento
  produtoCosif: string;           // Código composto ou ID do ProdutoCosif
}