import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MovimentoManual } from './movimento.model';

@Injectable({
  providedIn: 'root'
})
export class MovimentoManualService {
  private readonly baseUrl = 'http://localhost:5000/api/movimentomanual';

  constructor(private http: HttpClient) {}

  listarTodos(): Observable<MovimentoManual[]> {
    return this.http.get<MovimentoManual[]>(this.baseUrl);
  }

  incluir(movimento: MovimentoManual): Observable<void> {
    return this.http.post<void>(this.baseUrl, movimento);
  }

  atualizar(movimento: MovimentoManual): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/${movimento.mes}/${movimento.ano}/${movimento.numeroLancamento}`,
      movimento
    );
  }

  remover(movimento: MovimentoManual): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/${movimento.mes}/${movimento.ano}/${movimento.numeroLancamento}`
    );
  }
}
