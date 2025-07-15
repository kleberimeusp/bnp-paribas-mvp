import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MovimentoManual } from './movimento.model';

@Injectable({
  providedIn: 'root'
})
export class MovimentosService {

  private baseUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) {}

  // ==================== MOVIMENTOS ====================

  listarTodos(): Observable<MovimentoManual[]> {
    return this.http.get<MovimentoManual[]>(`${this.baseUrl}/movimentos/`);
  }

  incluir(movimento: MovimentoManual): Observable<any> {
    return this.http.post(`${this.baseUrl}/movimentos`, movimento);
  }

  atualizar(movimento: MovimentoManual): Observable<any> {
    return this.http.put(`${this.baseUrl}/movimentos/${movimento.numeroLancamento}`, movimento);
  }

  remover(numeroLancamento: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/movimentos/${numeroLancamento}`);
  }

  removerProdutoCosif(codigoProduto: string, codigoCosif: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/movimentos/${codigoProduto}/${codigoCosif}`);
  }
  

  // ==================== PRODUTOS ====================

  listarProdutos(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/produtos`);
  }

  // ==================== COSIFS ====================

  listarCosifs(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/produtoscosif`);
  }
}
