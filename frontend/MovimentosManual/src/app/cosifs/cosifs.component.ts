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
    this.cosifsService.listarCosifs().subscribe(data => {
      this.cosifs = data;
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
    this.cosif = {
      codigoCosif: c.codigoCosif.trim(),
      descricao: c.descricao,
      status: c.status
    };
    this.editando = true;
  }

  excluir(codigoCosif: string): void {
    if (confirm('Confirma exclusão?')) {
      this.cosifsService.remover(codigoCosif).subscribe(() => {
        this.listar();
        this.novo();
      });
    }
  }

  novo(): void {
    this.cosif = this.novoCosif();
    this.editando = false;
  }

  limpar(): void {
    this.cosif = this.novoCosif();
    this.editando = false;
  }

  private novoCosif(): Cosif {
    return {
      codigoCosif: '',
      descricao: '',
      status: 'A'
    };
  }
}
