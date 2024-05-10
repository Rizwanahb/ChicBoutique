export interface Category{
  id: number;
  categoryName: string;
  products?: (ProductsEntity)[] | null;
}

export interface ProductsEntity {
  id: number;
  title: string;
  price: number;
  description: string;
  image: string;
  stock: number;
}
