export interface Product{
  id:number;
  title:string;
  price:number;
  description:string;
  image:string;
  stock:number;
  categoryId?:number;
  category?: Category;
  [key: string]: any;  //  an index signature for dynamic property access
}

export interface Category {
  id: number;
  categoryName: string;
}

/*If your backend works fine and does not have any issues with the dynamic key in your Angular Product module,
and you don't have a corresponding key in your backend Product model, that's completely acceptable.
It means that your Angular frontend is using additional properties that are not part of the strict backend model,
and it's a common approach in some scenarios.

As long as your frontend and backend communication and data flow are well-understood and there are no data integrity or validation issues,
you can continue to use the dynamic key in your Angular Product module without a problem.*/
