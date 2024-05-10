import { Component, OnInit,ElementRef } from '@angular/core';
import { ProductService } from 'src/app/_services/product.service';
import { Product } from 'src/app/_models/product';
import { SearchService } from 'src/app/_services/search.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { SortPipe } from 'src/app/_pipes/sort.pipe';

@Component({
  selector: 'app-frontpage',
  templateUrl: './frontpage.component.html',
  styleUrls: ['./frontpage.component.css']
})
export class FrontpageComponent implements OnInit {
  products: Product[] = [];

  // string to store search value
  public searchTerm: string = "";
  sortCriteria: string = 'none'; // Initialize to 'none'
  sortDirection: string = 'asc'; // Initialize to 'asc'

  constructor(private productService: ProductService,
    private searchService: SearchService,
    private route: ActivatedRoute,
    private el: ElementRef,
    private router: Router) { }


navigateToAdminPage() {
  this.router.navigate(['/admin']);
}

  ngOnInit(): void {
    this.fetchProducts();
    // Subscribe to search term updates
    this.searchService.search.subscribe((searchTerm) => {
      this.searchTerm = searchTerm;
      this.searchProducts();
    });
  }

  //method for searching and filtering the list of products based on a search term
  searchProducts() {
    if (this.searchTerm == null || this.searchTerm == '' && (onkeyup)) {
      alert("The search field is empty")
      this.fetchProducts();
    }
    else if(this.searchTerm.length>= 0){
      this.productService.getAllProducts().subscribe((products: Product[]) => {
        // Use the filter method to filter products based on the searchTerm
        this.products = products.filter((product) => {
          // Check if the product's title or description contains the searchTerm (case-insensitive)
          const searchTerm = this.searchTerm.toLowerCase();
         return (
           product.title.toLowerCase().includes(searchTerm) ||
           product.description.toLowerCase().includes(searchTerm)
         );
        });
         // Check if no results were found
      if (this.products.length === 0) {
        alert("No results found");
      } else {
        // Results were found, scroll to the section
        this.scrollToSearchResultsSection();
      }
      });
    }}

  // Fetch a list of products from the ProductService
  private fetchProducts() {
    console.log("fetching product list");
    this.productService.getAllProducts().subscribe((products: Product[]) => {
      this.products = products;
    });
  }
  //Scrolls to the search results section of the front page component
  scrollToSearchResultsSection()
  {
    const element = this.el.nativeElement.querySelector('#search-results-section');
    if (element) {
      // Scroll to the element smoothly
      element.scrollIntoView({ behavior: 'smooth' });
    }
  }

  //Sort by Price function
  sortProducts(event: any) {
    const selectedValue = event.target.value;
    if (selectedValue === 'none') {
      this.sortCriteria = 'none';
    } else if (selectedValue === 'asc') {
      this.sortCriteria = 'price';
      this.sortDirection = 'asc';
    } else if (selectedValue === 'desc') {
      this.sortCriteria = 'price';
      this.sortDirection = 'desc';
    }
  }
}










