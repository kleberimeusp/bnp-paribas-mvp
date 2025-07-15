export interface MovimentoManual {
  mes: number;
  ano: number;
  numeroLancamento: number;
  codigoProduto: string;
  codigoCosif: string;
  descricao: string;
  valor: number;
  dataMovimento?: string;
  codigoUsuario: string;
  produtoCosif: string;
}
