import { Component, OnInit } from '@angular/core';
import { MovimentosService } from '../movimentos/movimentos.service';
import { MovimentoManual } from '../movimentos/movimento.model';
import { ProdutosService } from '../produtos/produtos.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  movimento: MovimentoManual = this.novoMovimento();
  movimentos: MovimentoManual[] = [];
  produtos: any[] = [];
  cosifs: any[] = [];
  editando = false;

  constructor(private movimentosService: MovimentosService,
              private produtosService: ProdutosService
  ) {}

  ngOnInit(): void {
    this.listar();
    this.carregarProdutos();
    this.carregarCosifs();
  }

  listar(): void {
    this.movimentosService.listarTodos().subscribe(data => {
      this.movimentos = data;
    });
  }

  carregarProdutos(): void {
    this.produtosService.listar().subscribe(data => {
      this.produtos = data;
      console.log(this.produtos);
    });
  }

  carregarCosifs(): void {
    this.movimentosService.listarCosifs().subscribe(data => {
      // Aplicar trim no código de cada cosif
      this.cosifs = data.map(c => ({
        ...c,
        codigoCosif: c.codigoCosif?.trim(),
        descricao: c.descricao?.trim()
      }));

      console.log('teste');
      console.log(this.cosifs);
    });
  }

  onSubmit(): void {
    this.movimento.dataMovimento = new Date();
    this.movimento.codigoUsuario = 'TESTE';

    if (this.editando && this.movimento.numeroLancamento) {
      this.movimentosService.atualizar(this.movimento).subscribe(() => {
        this.listar();
        this.movimento = this.novoMovimento();
        this.editando = false;
      });
    } else {
      this.movimentosService.incluir(this.movimento).subscribe(() => {
        this.listar();
        this.movimento = this.novoMovimento();
      });
    }
  }

  editar(mov: MovimentoManual): void {
    // Clona os dados e força string para os selects
    this.movimento = {
      ...mov,
      codigoProduto: mov.codigoProduto ? mov.codigoProduto.trim() : '',
      codigoCosif: mov.codigoCosif ? mov.codigoCosif.trim() : '',
    };

    // Garante listas carregadas
    if (this.produtos.length === 0) {
      this.carregarProdutos();
    }
    if (this.cosifs.length === 0) {
      this.carregarCosifs();
    }

    this.editando = true;

    console.log('teste 2')
    console.log(this.movimento)
  }

  excluir(numeroLancamento: number): void {
    if (confirm('Confirma exclusão?')) {
      this.movimentosService.remover(numeroLancamento).subscribe(() => {
        this.listar();
        this.novo();
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
    return {
      mes: new Date().getMonth() + 1,
      ano: new Date().getFullYear(),
      codigoProduto: '',
      codigoCosif: '',
      descricao: '',
      valor: 0
    };
  }
}
