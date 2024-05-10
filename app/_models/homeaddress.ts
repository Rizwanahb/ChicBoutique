import { AccountInfo } from "./accounInfo";


export interface HomeAddress {
    accountId: string;
    id: number;
    address: string;
    city: string;
    postalCode: string;
    country: string;
    phone: string;
    accountInfo: AccountInfo;
  }


