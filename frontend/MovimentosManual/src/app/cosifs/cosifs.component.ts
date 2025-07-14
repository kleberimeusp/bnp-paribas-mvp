import { Component, OnInit } from '@angular/core';
import { CosifsService } from './cosifs.service';
import { Cosif } from './cosif.model';

@Component({
  selector: 'app-cosifs',
  templateUrl: './cosifs.component.html',
  styleUrls: ['./cosifs.component.scss']
})
export class CosifsComponent implements OnInit {
  cosifs: Cosif[] = [];
  cosif: Cosif = this.novoCosif();
  editando = false;

  constructor(private cosifsService: CosifsService) {}

  ngOnInit(): void {
    this.listar();
  }

  listar(): void {
    this.cosifsService.listarTodos().subscribe({
      next: data => this.cosifs = data,
      error: err => console.error('Erro ao buscar cosifs', err)
    });
  }

  onSubmit(): void {
    if (this.editando) {
      this.cosifsService.atualizar(this.cosif).subscribe(() => {
        this.listar();
        this.novo();
      });
    } else {
      this.cosifsService.incluir(this.cosif).subscribe(() => {
        this.listar();
        this.novo();
      });
    }
  }

  editar(c: Cosif): void {
    this.cosif = { ...c };
    this.editando = true;
  }

  excluir(codigoProduto: string, codigoCosif: string): void {
    if (confirm('Confirma exclusão?')) {
      this.cosifsService.remover(codigoProduto).subscribe(() => this.listar());
    }
  }

  novo(): void {
    this.cosif = this.novoCosif();
    this.editando = false;
  }

  limpar(): void {
    this.novo();
  }

  private novoCosif(): Cosif {
    return {
      codigoProduto:'',
      codigoCosif: '',
      descricao: ''
    };
  }
}
