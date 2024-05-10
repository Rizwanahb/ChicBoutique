import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from 'src/app/_services/product.service';
import { Product } from 'src/app/_models/product';




@Component({
  selector: 'app-category-products',
  templateUrl: './category-products.component.html',
  styleUrls: ['./category-products.component.css']
})
export class CategoryProductsComponent implements OnInit {
  // Initialize the category ID to 0.
  categoryId: number = 0;
  // string to store search value
  public searchTerm: string = "";

  // Subscription to route parameters.
  private sub: any;

  // Array to store products.
  products: Product[] = [];

  // Constructor with injected services.
  constructor(private productService: ProductService, private route: ActivatedRoute) {
  }

  // Lifecycle hook called when the component is initialized.
  ngOnInit(): void {
    // Log a message to the console.
    console.log("This is the products-category page");

    // Subscribe to route parameter changes.
    this.sub = this.route.params.subscribe(params => {
      // Extract the 'id' parameter from the route.
      this.categoryId = +params['id'];

      // Fetch products by the category ID using the ProductService.
      this.productService.getProductsByCategoryId(this.categoryId).subscribe(products => {
        // Store the fetched products in the 'products' array.
        this.products = products;
      });
    });


  }


}
