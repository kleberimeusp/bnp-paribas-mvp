import { MovimentoManual } from '../movimentos/movimento.model';
import { Produto } from '../produtos/produto.model';

export interface Cosif {
  codigoProduto: string;
  codigoCosif: string;
  codigoClassificacao: string;
  status: string;
  produto?: any;
  movimentos?: any;

}