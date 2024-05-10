import { Category } from 'src/app/_models/product';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CategoryService } from 'src/app/_services/category.service';
import Swal from 'sweetalert2';
import { SearchService } from 'src/app/_services/search.service';  // SearchService

@Component({
  selector: 'app-admin-category',
  templateUrl: './admin-category.component.html',
  styleUrls: ['./admin-category.component.css']
})
export class AdminCategoryComponent implements OnInit {
  categoryForm!:FormGroup;
  categories: Category[] = [];
  category: Category = { id: 0, categoryName: '' }
  message!: string;
  searchTerm: string = '';

  constructor(private CategoryService:CategoryService, private fb: FormBuilder,
     private searchService: SearchService) { }


  ngOnInit(): void {
    this.CategoryService.getAllCategories().subscribe(
      c => {this.categories = c
        console.log(c);});

       // Subscribe to changes in the search term
     this.searchService.search$.subscribe((term) => {
      // Update the local searchTerm whenever the search term changes
      this.searchTerm = term;
      //  call the searchCategory() method.
      this.searchCategory();
  });
  }
       // method to handle input changes in the search field
       onSearchInputChange() {
        // Update the search term in the SearchService
        this.searchService.updateSearchTerm(this.searchTerm);
      }

   // method ot fetch all categories
   fetchCategories() {
    this.CategoryService.getAllCategories().subscribe((c)  => (this.categories= c));
  }

  //method to search specific category
  searchCategory() {

    if (this.searchTerm == null || this.searchTerm == '' && (onkeyup)) {
      alert('The search field is empty');
      this.fetchCategories();

    } else if (this.searchTerm.length >= 0) {
      this.CategoryService.getAllCategories().subscribe((categories: Category[]) => {
        // Use the filter method to filter categories based on the searchTerm
        this.categories = categories.filter((category) => {
          // Check if the category name contains the searchTerm (case-insensitive)
          const searchTerm = this.searchTerm.toLowerCase();
          return (
            category.categoryName.toLowerCase().includes(searchTerm)

          );
        });

        // Check if no results were found
        if (this.categories.length === 0) {
          alert('No results found');
        }

      });
    }
  }

  createCategoryForm(){
    this.categoryForm=this.fb.group({
      id: 0,
      categoryName: ['']
    })
  }

  cancel(): void {
    this.category = { id: 0, categoryName: '' }
  }


  edit(category: Category): void {
    this.message = '';
    this.category = category;
    this.category.id = category.id || 0;
    console.log(this.category);
  }
  delete(category: Category): void {
    if (confirm('Delete category: '+category.categoryName+'?')) {
      this.CategoryService.deleteCategory(category.id)
      .subscribe(() => {
        this.categories = this.categories.filter(cus => cus.id != category.id)
      })
    }
  }
 save(): void {
    console.log(this.category)
    this.message = '';

    if(this.category.id == 0) {
      this.CategoryService.addCategory(this.category)
      .subscribe({
        next: (x) => {
          console.log(x);
          this.categories.push(x);
          this.category = { id: 0, categoryName: '' }
          this.message = '';
          Swal.fire({
            title: 'Success!',
            text: 'Category added successfully',
            icon: 'success',
            confirmButtonText: 'OK'
          });
        },
        error: (err) => {
          console.log(err.error);
          this.message = Object.values(err.error.errors).join(", ");
        }
      });
    } else {
      this.CategoryService.editCategory(this.category.id, this.category)
      .subscribe({
        error: (err) => {
          console.log(err.error);
          this.message = Object.values(err.error.errors).join(", ");
        },
        complete: () => {
          this.message = '';
          this.category = { id: 0, categoryName: '' }
          Swal.fire({
            title: 'Success!',
            text: 'Category updated successfully',
            icon: 'success',
            confirmButtonText: 'OK'
          });
        }
      });
    }
  }
}
