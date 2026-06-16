export interface CreateAddressModel {
  registryNumber: string;
  city: string;
  street: string;
  houseNumber: string;
  apartmentNumber: string;
  postalCode: string;
  country: string;
  documentName: string;
}

export interface UpdateAddressModel {
  registryNumber?: string;
  city?: string;
  street?: string;
  houseNumber?: string;
  apartmentNumber?: string;
  postalCode?: string;
  country?: string;
  documentName?: string;
}

export interface DeleteAddressModel {
  addressId: number;
}
