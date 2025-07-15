export interface Produto {
  codigoProduto: string;
  descricao: string;
  status: string;
  produtosCosif?: any[]; // ou ProdutoCosif[] se tiver interface definida
}
