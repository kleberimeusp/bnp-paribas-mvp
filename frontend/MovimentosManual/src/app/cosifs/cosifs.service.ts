import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cosif } from './cosif.model';

@Injectable({
  providedIn: 'root'
})
export class CosifsService {
  private baseUrl = 'http://localhost:5000/api/produtocosifs'; // ⚠️ Confirme se esta URL existe no backend

  constructor(private http: HttpClient) {}

  listarTodos(): Observable<Cosif[]> {
    return this.http.get<Cosif[]>(`${this.baseUrl}`);
  }

  incluir(cosif: Cosif): Observable<any> {
    return this.http.post(this.baseUrl, cosif);
  }

  atualizar(cosif: Cosif): Observable<any> {
    return this.http.put(`${this.baseUrl}/${cosif.codigoCosif}`, cosif);
  }

  remover(codigo: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${codigo}`);
  }
}
