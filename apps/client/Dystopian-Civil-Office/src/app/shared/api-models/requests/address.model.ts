export interface CreateAddressModel {
  city: string;
  street: string;
  houseNumber: string;
  apartmentNumber: string;
  postalCode: string;
  country: string;
}

export interface UpdateAddressModel {
  city?: string;
  street?: string;
  houseNumber?: string;
  apartmentNumber?: string;
  postalCode?: string;
  country?: string;
}

export interface DeleteAddressModel {
  addressId: number;
}
