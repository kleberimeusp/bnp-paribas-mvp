import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Produto } from './produto.model';

@Injectable({
  providedIn: 'root'
})
export class ProdutosService {
  private baseUrl = 'http://localhost:5000/api/produtos';

  constructor(private http: HttpClient) {}

  listar(): Observable<Produto[]> {
    return this.http.get<Produto[]>(`${this.baseUrl}`);
  }

  incluir(produto: Produto): Observable<any> {
    return this.http.post(this.baseUrl, produto);
  }

  atualizar(produto: Produto): Observable<any> {
    return this.http.put(`${this.baseUrl}/${produto.codigo}`, produto);
  }

  remover(codigo: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${codigo}`);
  }
}
