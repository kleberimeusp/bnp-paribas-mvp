import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cosif } from './cosif.model';

@Injectable({
  providedIn: 'root'
})
export class CosifsService {
  private readonly baseUrl = 'http://localhost:5000/api/Cosif';

  constructor(private http: HttpClient) {}

  listarCosifs(): Observable<Cosif[]> {
    return this.http.get<Cosif[]>(`${this.baseUrl}`);
  }

  buscarPorCodigo(codigo: string): Observable<Cosif> {
    return this.http.get<Cosif>(`${this.baseUrl}/${codigo}`);
  }

  incluir(cosif: Cosif): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}`, cosif);
  }

  atualizar(cosif: Cosif): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${cosif.codigoCosif}`, cosif);
  }

  remover(codigo: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${codigo}`);
  }

  listarPaginado(pagina: number, tamanho: number): Observable<Cosif[]> {
    return this.http.get<Cosif[]>(`${this.baseUrl}/paged?page=${pagina}&size=${tamanho}`);
  }
}
