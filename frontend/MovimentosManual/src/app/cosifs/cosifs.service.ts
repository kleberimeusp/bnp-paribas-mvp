import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cosif } from './cosif.model';
import { Produto } from '../produtos/produto.model';

@Injectable({
  providedIn: 'root'
})
export class CosifsService {
  private baseUrl = 'http://localhost:5000/api/produtoscosif';
  private produtosUrl = 'http://localhost:5000/api/produtos';

  produtos: Produto[] = []; // ✅ armazenar produtos localmente após carregar

  constructor(private http: HttpClient) {}

  listarCosifs(): Observable<Cosif[]> {
    return this.http.get<Cosif[]>(this.baseUrl);
  }

  listarProdutos(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.produtosUrl);
  }

  /**
   * Método para buscar um produto localmente no array de produtos carregados.
   */
  findProduto(codigoProduto: string): Produto | undefined {
    return this.produtos.find(p => p.codigoProduto === codigoProduto);
  }

  private contadorProduto = 1;
  private contadorCosif = 1;
  private contadorLancamento = 1;

  incluir(cosif: Cosif): Observable<any> {
    // Garante os tamanhos e preenche dados
    cosif.codigoProduto = `P${String(this.contadorProduto).padStart(3, '0')}`.substring(0, 4);
    cosif.codigoCosif = `C${String(this.contadorCosif).padStart(4, '0')}`.substring(0, 11);
    cosif.codigoClassificacao = cosif.codigoClassificacao?.trim().substring(0, 6) || 'CL01';
    cosif.status = cosif.status?.trim().substring(0, 1) || 'A';

    // Monta objeto produto
    cosif.produto = {
      codigoProduto: cosif.codigoProduto,
      descricao: cosif.produto?.descricao || `Produto ${this.contadorProduto}`,
      status: cosif.status,
      produtosCosif: []
    };

    // Cria movimento com número de lançamento incremental
    const movimento = {
      mes: new Date().getMonth() + 1,
      ano: new Date().getFullYear(),
      numeroLancamento: this.contadorLancamento,
      codigoProduto: cosif.codigoProduto,
      codigoCosif: cosif.codigoCosif,
      descricao: `Movimento ${this.contadorLancamento}`,
      dataMovimento: new Date().toISOString(),
      codigoUsuario: `USR${this.contadorLancamento}`,
      valor: 100.0
    };

    cosif.movimentos = [movimento];

    // ✅ Printar JSON formatado no console
    console.log('JSON enviado para o backend:', JSON.stringify(cosif, null, 2));

    // Incrementar contadores
    this.contadorProduto++;
    this.contadorCosif++;
    this.contadorLancamento++;

    return this.http.post(this.baseUrl, cosif);
  }





  atualizar(cosif: Cosif): Observable<any> {
    // Garante que os campos não ultrapassem os limites do banco
    cosif.codigoProduto = cosif.codigoProduto.trim().substring(0, 4);
    cosif.codigoCosif = cosif.codigoCosif.trim().substring(0, 11);
    cosif.codigoClassificacao = cosif.codigoClassificacao?.trim().substring(0, 6);
    cosif.status = cosif.status.trim().substring(0, 1);

    // Monta objeto produto
    cosif.produto = {
      codigoProduto: cosif.codigoProduto,
      descricao: cosif.produto?.descricao || '', // ou preenche do combo se tiver
      status: cosif.status,
      produtosCosif: [] // envia vazio ou conforme necessidade
    };

    // Exemplo movimento preenchido (pode ser vazio se não usar)
    const movimentoExemplo = {
      mes: new Date().getMonth() + 1,
      ano: new Date().getFullYear(),
      numeroLancamento: 1,
      codigoProduto: cosif.codigoProduto,
      codigoCosif: cosif.codigoCosif,
      descricao: "Movimento teste",
      dataMovimento: new Date().toISOString(),
      codigoUsuario: "USR1",
      valor: 100.0
    };

    // Se você não quiser enviar movimentos, comente a linha abaixo
    cosif.movimentos = [movimentoExemplo];

    return this.http.put(`${this.baseUrl}/${cosif.codigoProduto}/${cosif.codigoCosif}`, cosif);
  }




  remover(codigoProduto: string, codigoCosif: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${codigoProduto}/${codigoCosif}`);
  }
}
