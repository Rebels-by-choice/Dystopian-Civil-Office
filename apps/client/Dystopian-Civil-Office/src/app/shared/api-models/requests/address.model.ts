export interface CreateAddressModel {
  registryNumber: string;
  city: string;
  street: string;
  houseNumber: string;
  apartmentNumber: string;
  postalCode: string;
  country: string;
  documentName: number;
}

export interface UpdateAddressModel {
  registryNumber?: string;
  city?: string;
  street?: string;
  houseNumber?: string;
  apartmentNumber?: string;
  postalCode?: string;
  country?: string;
  documentName?: number;
}

export interface DeleteAddressModel {
  addressId: number;
}
