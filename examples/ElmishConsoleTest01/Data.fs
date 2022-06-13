module Data 

open System.Data
open System.IO

let xml = """

<NewDataSet>
<xs:schema xmlns="" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msdata="urn:schemas-microsoft-com:xml-msdata" id="NewDataSet">
<xs:element name="NewDataSet" msdata:IsDataSet="true" msdata:MainDataTable="RecentMatter" msdata:UseCurrentLocale="true">
<xs:complexType>
<xs:choice minOccurs="0" maxOccurs="unbounded">
<xs:element name="RecentMatter">
<xs:complexType>
<xs:sequence>
<xs:element name="UserLogin">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="MatterNumber">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="ClientName">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="MatterName">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="ClientCode" minOccurs="0">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="OfficeCode" minOccurs="0">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="OfficeName" minOccurs="0">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="Billable" type="xs:boolean"/>
<xs:element name="ReferenceId" minOccurs="0">
<xs:simpleType>
<xs:restriction base="xs:string">
<xs:maxLength value="2147483647"/>
</xs:restriction>
</xs:simpleType>
</xs:element>
<xs:element name="LastUsed" type="xs:dateTime"/>
</xs:sequence>
</xs:complexType>
</xs:element>
</xs:choice>
</xs:complexType>
<xs:unique name="Constraint1" msdata:PrimaryKey="true">
<xs:selector xpath=".//RecentMatter"/>
<xs:field xpath="UserLogin"/>
<xs:field xpath="MatterNumber"/>
</xs:unique>
</xs:element>
</xs:schema>

<RecentMatter>
  <UserLogin>PSLTP6\RJK</UserLogin>
  <MatterNumber>99999-2302</MatterNumber>
  <ClientName>Test Matters</ClientName>
  <MatterName>DP Test Matter</MatterName>
  <ClientCode>99999</ClientCode>
  <OfficeCode/>
  <OfficeName/>
  <Billable>true</Billable>
  <ReferenceId/>
  <LastUsed>2011-08-23T23:40:24.13+01:00</LastUsed>
</RecentMatter>
<RecentMatter>
  <UserLogin>PSLTP6\RJK</UserLogin>
  <MatterNumber>999991.0002</MatterNumber>
  <ClientName>Lathe 1</ClientName>
  <MatterName>LW Test 2</MatterName>
  <ClientCode/>
  <OfficeCode/>
  <OfficeName/>
  <Billable>true</Billable>
  <ReferenceId/>
  <LastUsed>2011-07-12T16:57:27.173+01:00</LastUsed>
</RecentMatter>
<RecentMatter>
  <UserLogin>PSLTP6\RJK</UserLogin>
  <MatterNumber>999991-0001</MatterNumber>
  <ClientName>Lathe 1</ClientName>
  <MatterName>LW Test 1</MatterName>
  <ClientCode/>
  <OfficeCode/>
  <OfficeName/>
  <Billable>false</Billable>
  <ReferenceId/>
  <LastUsed>2011-07-12T01:59:06.887+01:00</LastUsed>
</RecentMatter>
</NewDataSet>

    """

let table = 
    use sr = new StringReader(xml)
    let dt = new DataTable()
    dt.ReadXml(sr) |> ignore
    dt

