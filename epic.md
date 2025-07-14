Initial Structure for BNP Paribas & Antlia Challenge:

# Backend - Spring Boot

Package Structure:
```
com.antlia.challenge
├── ChallengeApplication.java
├── config
│   └── CorsConfig.java
├── controller
│   └── ManualMovementController.java
│   └── ProductController.java
├── dto
│   └── ManualMovementDTO.java
│   └── ProductDTO.java
├── entity
│   └── ManualMovement.java
│   └── Product.java
│   └── ProductCosif.java
├── repository
│   └── ManualMovementRepository.java
│   └── ProductRepository.java
│   └── ProductCosifRepository.java
├── service
│   └── ManualMovementService.java
│   └── ProductService.java
```

pom.xml Dependencies:
```xml
<dependencies>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-web</artifactId>
    </dependency>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-data-jpa</artifactId>
    </dependency>
    <dependency>
        <groupId>com.h2database</groupId>
        <artifactId>h2</artifactId>
        <scope>runtime</scope>
    </dependency>
    <dependency>
        <groupId>org.springframework.boot</groupId>
        <artifactId>spring-boot-starter-test</artifactId>
        <scope>test</scope>
    </dependency>
</dependencies>
```

Database: H2 configured in application.properties
```properties
spring.datasource.url=jdbc:h2:mem:testdb
spring.datasource.driverClassName=org.h2.Driver
spring.datasource.username=sa
spring.datasource.password=
spring.jpa.hibernate.ddl-auto=update
spring.h2.console.enabled=true
```

Example Entity Product.java:
```java
@Entity
@Table(name = "PRODUTO")
public class Product {
    @Id
    @Column(name = "COD_PRODUTO", length = 4)
    private String productCode;

    @Column(name = "DES_PRODUTO", length = 30, nullable = false)
    private String productDescription;

    @Column(name = "STA_STATUS", length = 1, nullable = false)
    private String status;
}
```

Example Entity ProductCosif.java:
```java
@Entity
@Table(name = "PRODUTO_COSIF")
public class ProductCosif {
    @Id
    @Column(name = "COD_COSIF", length = 11)
    private String cosifCode;

    @Column(name = "COD_CLASSIFICACAO", length = 6, nullable = false)
    private String classificationCode;

    @Column(name = "STA_STATUS", length = 1, nullable = false)
    private String status;

    @ManyToOne
    @JoinColumn(name = "COD_PRODUTO", nullable = false)
    private Product product;
}
```

Example Entity ManualMovement.java:
```java
@Entity
@Table(name = "MOVIMENTO_MANUAL")
@IdClass(ManualMovementId.class)
public class ManualMovement {

    @Id
    @Column(name = "DAT_MES")
    private Integer month;

    @Id
    @Column(name = "DAT_ANO")
    private Integer year;

    @Id
    @Column(name = "NUM_LANCAMENTO")
    private Integer movementNumber;

    @ManyToOne
    @JoinColumn(name = "COD_PRODUTO", nullable = false)
    private Product product;

    @ManyToOne
    @JoinColumn(name = "COD_COSIF", nullable = false)
    private ProductCosif productCosif;

    @Column(name = "DES_DESCRICAO", length = 60, nullable = false)
    private String description;

    @Column(name = "DAT_MOVIMENTO", length = 10, nullable = false)
    private String movementDate;

    @Column(name = "COD_USUARIO", length = 15, nullable = false)
    private String userCode;

    @Column(name = "VAL_VALOR", precision = 11, scale = 2, nullable = false)
    private Double amount;
}
```

ManualMovementId.java:
```java
import java.io.Serializable;
import java.util.Objects;

public class ManualMovementId implements Serializable {

    private Integer month;
    private Integer year;
    private Integer movementNumber;

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        ManualMovementId that = (ManualMovementId) o;
        return Objects.equals(month, that.month) &&
               Objects.equals(year, that.year) &&
               Objects.equals(movementNumber, that.movementNumber);
    }

    @Override
    public int hashCode() {
        return Objects.hash(month, year, movementNumber);
    }
}
```

# Frontend - Angular 16+

Structure:
```
/src/app
├── app.module.ts
├── app.component.ts
├── app.component.html
├── movements
│   ├── movements.component.ts
│   ├── movements.component.html
│   └── movements.component.css
├── services
│   ├── movements.service.ts
│   └── products.service.ts
```

Key Dependencies (package.json):
```json
"dependencies": {
  "@angular/common": "^16.0.0",
  "@angular/core": "^16.0.0",
  "@angular/forms": "^16.0.0",
  "@angular/platform-browser": "^16.0.0",
  "@angular/platform-browser-dynamic": "^16.0.0",
  "@angular/router": "^16.0.0",
  "rxjs": "^7.0.0",
  "zone.js": "^0.13.0"
}
```

movements.component.html:
```html
<form>
  <input placeholder="Month" [(ngModel)]="month">
  <input placeholder="Year" [(ngModel)]="year">
  <select [(ngModel)]="product">
    <option *ngFor="let p of products" [value]="p.productCode">{{p.productDescription}}</option>
  </select>
  <select [(ngModel)]="cosif">
    <option *ngFor="let c of cosifs" [value]="c.cosifCode">{{c.cosifCode}}</option>
  </select>
  <input placeholder="Description" [(ngModel)]="description">
  <input placeholder="Amount" [(ngModel)]="amount" type="number">
  <button (click)="newMovement()">New</button>
  <button (click)="add()">Add</button>
  <button (click)="clear()">Clear</button>
</form>

<table>
  <tr>
    <th>Month</th><th>Year</th><th>Product</th><th>Product Description</th><th>Movement No.</th><th>Description</th><th>Amount</th>
  </tr>
  <tr *ngFor="let m of movements">
    <td>{{m.month}}</td><td>{{m.year}}</td><td>{{m.productCode}}</td><td>{{m.productDescription}}</td><td>{{m.movementNumber}}</td><td>{{m.description}}</td><td>{{m.amount}}</td>
  </tr>
</table>
```

This skeleton implements the full foundation for the backend entities and frontend structure, respecting primary keys, foreign keys, and the specifications of the challenge.
