
// produtos.component.ts
import { Component, OnInit } from '@angular/core';
import { Produto } from './produto.model';
import { ProdutosService } from './produtos.service';

@Component({
  selector: 'app-produtos',
  templateUrl: './produtos.component.html',
  styleUrls: ['./produtos.component.scss']
})
export class ProdutosComponent implements OnInit {
  produtos: Produto[] = [];
  produto: Produto = this.novoProduto();
  editando = false;

  constructor(private produtosService: ProdutosService) {}

  ngOnInit(): void {
    this.listar();
  }

  listar(): void {
    this.produtosService.listar().subscribe(data => 
      this.produtos = data
    );
  }

  salvar(): void {
    if (this.editando) {
      this.produtosService.atualizar(this.produto).subscribe(() => {
        this.listar();
        this.produto = this.novoProduto();
        this.editando = false;
      });
    } else {
      this.produtosService.incluir(this.produto).subscribe(() => {
        this.listar();
        this.produto = this.novoProduto();
      });
    }
  }

  editar(p: Produto): void {
    this.produto = { ...p };
    this.editando = true;
  }

  excluir(codigo: string): void {
    if (confirm('Confirma exclusão?')) {
      this.produtosService.remover(codigo).subscribe(() => {
        this.listar();
        this.novo();
      });
    }
  }

  novo(): void {
    this.produto = this.novoProduto();
    this.editando = false;
  }

  limpar(): void {
    this.produto = this.novoProduto();
    this.editando = false;
  }

  private novoProduto(): Produto {
    return {
      codigoProduto: '',
      descricao: '',
      status: 'A',
      produtosCosif: []
    };
  }
}
