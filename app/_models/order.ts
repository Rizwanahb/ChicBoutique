
import { CartItem } from "./cartItem";
import { ShippingDetails } from "./shippingdetails";

interface OrderDetail {
  id: number;
  productId: number;
  productTitle: string;
  productPrice: number;
  quantity: number;
}

export interface Order {

    id?: number;
    accountInfoId: string;
    clientBasketId: string;
    orderDate?: Date;
    amount: number;
    userId?:number;
    transactionId: string;
    status: string;
    paymentMethod:string;
    timePaid?:Date;
    shippingDetails: ShippingDetails;

    orderDetails: CartItem[];


}

