import { AccountInfo } from "./accounInfo";
import { HomeAddress } from "./homeaddress";
import { Role } from "./role";

import { ShippingDetails } from "./shippingdetails";

export interface User {

    id: number;
    email: string;
    firstName: string;
    lastName: string;
    password?:string;
    address: string;
    city: string;
    postalcode: string;
    country: string;
    telephone: string;
    role: Role;
    accountInfo?:AccountInfo;
    homeAddress?: HomeAddress;
    shippingDetails?: ShippingDetails; // Optional shipping details
    orders?: Order[];  // Array of orders associated with the user
}

// order.model.ts
export interface Order {

  userId?: number;  // Link order to a user
}
