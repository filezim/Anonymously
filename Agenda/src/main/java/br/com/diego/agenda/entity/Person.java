package br.com.diego.agenda.entity;

import javax.persistence.Column;
import javax.persistence.Entity;

@SuppressWarnings("serial")
@Entity
public class Person extends GenericEntity {
	@Column(length=50, nullable=false)
	private String name;
	@Column(length=15, nullable=false)
	private String identy;
	@Column(length=15, nullable=false)
	private String cpf;
	@Column(length=15, nullable=false)
	private String landLine;
	@Column(length=17, nullable=false)
	private String cellPhone;
	@Column(length=30, nullable=false)
	private String address;
	@Column(length=10, nullable=false)
	private String postalCode;
	@Column(length=20, nullable=false)
	private String district;
	@Column(length=50, nullable=false)
	private String email;
	@Column(length=12, nullable=false)
	private String city;
	@Column(length=20, nullable=false)
	private String complement;

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getIdenty() {
		return identy;
	}

	public void setIdenty(String identy) {
		this.identy = identy;
	}

	public String getCpf() {
		return cpf;
	}

	public void setCpf(String cpf) {
		this.cpf = cpf;
	}

	public String getLandLine() {
		return landLine;
	}

	public void setLandline(String landLine) {
		this.landLine = landLine;
	}

	public String getCellPhone() {
		return cellPhone;
	}

	public void setCellPhone(String cellPhone) {
		this.cellPhone = cellPhone;
	}

	public String getAddress() {
		return address;
	}

	public void setAddress(String address) {
		this.address = address;
	}

	public String getPostalCode() {
		return postalCode;
	}

	public void setPostalCode(String postalCode) {
		this.postalCode = postalCode;
	}

	public String getDistrict() {
		return district;
	}

	public void setDistrict(String district) {
		this.district = district;
	}

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getCity() {
		return city;
	}

	public void setCity(String city) {
		this.city = city;
	}

	public String getComplement() {
		return complement;
	}

	public void setComplement(String complement) {
		this.complement = complement;
	}

}
