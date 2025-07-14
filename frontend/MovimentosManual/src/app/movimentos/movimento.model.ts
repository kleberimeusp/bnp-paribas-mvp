export interface MovimentoManual {
  mes: number;
  ano: number;
  codigoProduto: string;
  codigoCosif: string;
  descricao: string;
  valor: number;
  numeroLancamento?: number;
  dataMovimento?: Date;
  codigoUsuario?: string;
}
