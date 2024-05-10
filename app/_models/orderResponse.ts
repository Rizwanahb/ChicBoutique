
import { AccountInfo } from "./accounInfo";
import { CartItem } from "./cartItem";
import { ShippingDetails } from "./shippingdetails";



export interface OrderResponse {

    id?: number;
    accountInfoId: string;
    
    orderDate?: Date;
    amount: number;
    userId?:number;
    transactionId: string;
    status: string;
    paymentMethod:string;
    timePaid?:Date;
    shippingDetails: ShippingDetails;
    accountInfo:AccountInfo;
    orderDetails: CartItem[];


}

