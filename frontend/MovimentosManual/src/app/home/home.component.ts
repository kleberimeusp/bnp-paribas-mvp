import { Component, OnInit } from '@angular/core';
import { MovimentoManual } from '../movimentos/movimento.model';
import { Produto } from '../produtos/produto.model';
import { Cosif } from '../cosifs/cosif.model';
import { MovimentoManualService } from '../movimentos/movimento-manual.service';
import { ProdutosService } from '../produtos/produtos.service';
import { CosifsService } from '../cosifs/cosifs.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  movimentos: MovimentoManual[] = [];
  produtos: Produto[] = [];
  cosifs: Cosif[] = [];

  movimento: MovimentoManual = this.novoMovimento();
  editando = false;

  constructor(
    private movimentoService: MovimentoManualService,
    private produtoService: ProdutosService,
    private cosifService: CosifsService
  ) {}

  ngOnInit(): void {
    this.carregarMovimentos();
    this.carregarProdutos();
    this.carregarCosifs();
  }

  carregarMovimentos(): void {
    this.movimentoService.listarTodos().subscribe({
      next: data => {
        this.movimentos = data.map(m => ({
          ...m,
          codigoProduto: m.codigoProduto?.trim(),
          codigoCosif: m.codigoCosif?.trim(),
          descricao: m.descricao?.trim()
        }));
      },
      error: err => console.error('Erro ao carregar movimentos:', err)
    });
  }

  carregarProdutos(): void {
    this.produtoService.listar().subscribe({
      next: data => {
        this.produtos = data.map(p => ({
          ...p,
          codigoProduto: p.codigoProduto?.trim(),
          descricao: p.descricao?.trim()
        }));
      },
      error: err => console.error('Erro ao carregar produtos:', err)
    });
  }

  carregarCosifs(): void {
    this.cosifService.listarCosifs().subscribe({
      next: data => {
        this.cosifs = data.map(c => ({
          ...c,
          codigoCosif: c.codigoCosif?.trim(),
          descricao: c.descricao?.trim()
        }));
      },
      error: err => console.error('Erro ao carregar cosifs:', err)
    });
  }

  salvar(): void {
    if (!this.movimento.codigoProduto || !this.movimento.codigoCosif || !this.movimento.descricao || !this.movimento.valor) {
      alert('Todos os campos obrigatórios devem ser preenchidos.');
      return;
    }

    this.movimento.codigoUsuario = 'admin';
    this.movimento.produtoCosif = `${this.movimento.codigoProduto}-${this.movimento.codigoCosif}`;
    this.movimento.dataMovimento = new Date().toISOString();

    if (this.editando) {
      this.movimentoService.atualizar(this.movimento).subscribe({
        next: () => {
          this.carregarMovimentos();
          this.limpar();
        },
        error: err => alert('Erro ao atualizar: ' + (err.error?.erro || err.message))
      });
    } else {
      this.movimentoService.incluir(this.movimento).subscribe({
        next: () => {
          this.carregarMovimentos();
          this.limpar();
        },
        error: err => alert('Erro ao incluir: ' + (err.error?.erro || err.message))
      });
    }
  }

  editar(mov: MovimentoManual): void {
    this.movimento = { ...mov };
    this.editando = true;
  }

  excluirMovimento(mov: MovimentoManual): void {
    if (confirm(`Deseja remover o movimento "${mov.descricao}"?`)) {
      this.movimentoService.remover(mov).subscribe({
        next: () => this.carregarMovimentos(),
        error: err => alert('Erro ao remover: ' + (err.error?.erro || err.message))
      });
    }
  }

  novo(): void {
    this.movimento = this.novoMovimento();
    this.editando = false;
  }

  limpar(): void {
    this.movimento = this.novoMovimento();
    this.editando = false;
  }

  private novoMovimento(): MovimentoManual {
    const agora = new Date();
    return {
      mes: (agora.getMonth() + 1).toString(),
      ano: agora.getFullYear().toString(),
      numeroLancamento: 0,
      codigoProduto: '',
      codigoCosif: '',
      descricao: '',
      valor: 0,
      codigoUsuario: '',
      produtoCosif: '',
      dataMovimento: agora.toISOString()
    };
  }
}
